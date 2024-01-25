using System.Text.RegularExpressions;
using ClassNegarService.Models.Class;
using ClassNegarService.Repos;
using ClassNegarService.Repos.Notification;
using ClassNegarService.Repos.Session;
using ClassNegarService.Utils;

namespace ClassNegarService.Services
{
    public class ClassService : IClassService
    {
        private readonly ISessionRepo _sessionRepo;
        private readonly IClassRepo _classRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly IConfiguration _configuration;

        public ClassService(
            IClassRepo classRepo,
                        ISessionRepo sessionRepo,

            INotificationRepo notificationRepo,
            IConfiguration configuration
            )
        {
            _sessionRepo = sessionRepo;
            _classRepo = classRepo;
            _notificationRepo = notificationRepo;
            _configuration = configuration;
        }

        public async Task<AddClassResponseModel> AddClass(AddClassModel model, int professorId)
        {
            var code = GenerateClassCode(model.Name, model.Semester, professorId);
            var password = StringUtils.RandomString(8);
            var classId = await _classRepo.AddClass(model, code, password, professorId) ?? throw new Exception();
            await _classRepo.AddTimeToClass(model.Times, classId);
            return new AddClassResponseModel { Code = code, Password = password };
        }

        public async Task AddClassResourse(AddClassResourse model, int professorId)
        {
            var hasAccess = await _classRepo.HasProfessorAccess(professorId, model.ClassId);
            if (hasAccess == false) throw new UnauthorizedAccessException();

            if (string.IsNullOrEmpty(model.Base64Data)) throw new InvalidDataException();

            var file = model.Base64Data.Split(',');
            if (file.Length != 2) throw new InvalidDataException();
            var fileFormat = GetFileFormat(file[0]);
            if (fileFormat == null) throw new InvalidDataException();
            var path = GenerateFilePath(model, fileFormat);
            var content = Convert.FromBase64String(file[1]);
            var directoryPath = _configuration["BaseDirectoryPath"] + path;
            var downloadPath = _configuration["BaseUrlPath"] + path;

            await File.WriteAllBytesAsync(directoryPath, content);

            await _classRepo.AddClassResourses(model.Name, model.ClassId, downloadPath, DateTime.Now, fileFormat, content.Length);





        }

        public async Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId)
        {
            var result = await _classRepo.GetAllProfessorClasses(professorId);
            return result;
        }

        public async Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId)
        {
            var result = await _classRepo.GetAllStudentClasses(studentId);
            return result;
        }

        public async Task<ProfessorClassesModel?> GetProfessorClass(int professorId, int classId)
        {
            var result = await _classRepo.GetProfessorClass(professorId, classId) ?? throw new UnauthorizedAccessException();
            result.ClassTimes = await _classRepo.GetClassTimes(classId);
            result.Notifications = await _notificationRepo.GetClassNotifications(classId);
            return result;
        }

        public async Task<List<ClassResourseModel>> GetProfessorResources(int professorId, int classId)
        {
            var hasAccess = await _classRepo.HasProfessorAccess(professorId, classId);
            if (hasAccess == false) throw new UnauthorizedAccessException();

            var result = await _classRepo.GetClassResourses(classId);
            return result ?? throw new UnauthorizedAccessException();

        }

        public async Task<StudentClassesModel?> GetStudentClass(int studentId, int classId)
        {
            var hasEnrolled = await _classRepo.HasEnrolled(studentId, classId);
            if (hasEnrolled == false) throw new UnauthorizedAccessException();

            var result = await _classRepo.GetStudentClass(classId) ?? throw new UnauthorizedAccessException();
            result.ClassTimes = await _classRepo.GetClassTimes(classId);
            result.Notifications = await _notificationRepo.GetClassNotifications(classId);
            return result;
        }

        public async Task<List<ClassResourseModel>> GetStudentResources(int studentId, int classId)
        {
            var hasAccess = await _classRepo.HasEnrolled(studentId, classId);
            if (hasAccess == false) throw new UnauthorizedAccessException();

            var result = await _classRepo.GetClassResourses(classId);
            return result ?? throw new UnauthorizedAccessException();
        }

        public async Task JoinClass(JoinClassModel model, int studentId)
        {
            var classId = await _classRepo.GetClassId(model) ?? throw new UnauthorizedAccessException();
            var hasEnrolledBefore = await _classRepo.HasEnrolled(studentId, classId);
            if (hasEnrolledBefore) throw new Exception("you are already enrolled to the class");
            await _classRepo.AddEnrollment(studentId, classId, DateTime.Now);

        }

        string GenerateClassCode(string name, DateTime semester, int professorId)
        {

            string inputString = $"{name}{semester.ToString("yyyyMMdd")}{professorId}";
            int hashCode = inputString.GetHashCode();
            hashCode = Math.Abs(hashCode);
            string classCode = hashCode.ToString("D8");
            return classCode;

        }

        string GetFileFormat(string data)
        {
            try
            {
                Regex rg = new Regex(@"(?<=/)(.*)(?=;)");

                var type = rg.Matches(data)[0];
                return type.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        string GenerateFilePath(AddClassResourse model, string fileFormat)
        {

            string filename = StringUtils.RandomString(4) + model.ClassId + model.Name;
            int hash = filename.GetHashCode();
            hash = Math.Abs(hash);
            string pre = hash.ToString("D8").Replace(" ", "");
            string filePath = pre + "_" + model.ClassId + $".{fileFormat}";
            return filePath;
        }

        public async Task<List<string>> TodayClassPresent(int professorId, int classId)
        {
            var hasAccess = await _classRepo.HasProfessorAccess(professorId, classId);
            if (hasAccess == false) throw new UnauthorizedAccessException();
            var now = DateTime.Now;
            var latestSession = await _sessionRepo.GetLatestSessionDate(classId);
            if (latestSession == null)
            {
                throw new InvalidDataException();

            }
            var sessionDate = latestSession.StartedAt;
            if (sessionDate.Year != now.Year || sessionDate.Month != now.Month || sessionDate.Day != now.Day)
            {
                throw new InvalidDataException();
            }
            var listresult = await _sessionRepo.GetSessionPresent(latestSession.Id);

            return listresult;

        }

        public async Task<List<AdminClassModel>?> GetAllAdminCurrentClasses()
        {
            var result = await _classRepo.GetAllCurrentClasses();
            return result;
        }

        public async Task<List<AdminClassModel>?> GetAllAdminDoneClasses()
        {
            var result = await _classRepo.GetAllDoneClasses();
            return result;
        }

        public async Task<List<AdminClassCalendarModel>?> GetAdminClassCalendar()
        {
            var result = await _classRepo.GetAdminClassCalendar();
            return result;
        }

        public async Task<List<AdminReportClassModel>?> GetAllAdminReportClasses()
        {
            var result = await _classRepo.GetAllAdminReportClasses();
            return result;
        }
    }
}

