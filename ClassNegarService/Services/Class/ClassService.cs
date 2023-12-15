using ClassNegarService.Models.Class;
using ClassNegarService.Repos;
using ClassNegarService.Utils;

namespace ClassNegarService.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepo _classRepo;
        private readonly IConfiguration _configuration;

        public ClassService(
            IClassRepo classRepo,
            IConfiguration configuration
            )
        {
            _classRepo = classRepo;
            _configuration = configuration;
        }

        public async Task AddClass(AddClassModel model, int professorId)
        {
            var code = GenerateClassCode(model.Name, model.Semester, professorId);
            var password = StringUtils.RandomString(8);
            var classId = await _classRepo.AddClass(model, code, password, professorId) ?? throw new Exception();
            await _classRepo.AddTimeToClass(model.Times, classId);
        }

        public Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId)
        {
            var result = _classRepo.GetAllProfessorClasses(professorId);
            return result;
        }

        public Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId)
        {
            var result = _classRepo.GetAllStudentClasses(studentId);
            return result;
        }

        public async Task JoinClass(JoinClassModel model, int studentId)
        {
            var classId = await _classRepo.GetClassId(model) ?? throw new UnauthorizedAccessException();
            await _classRepo.AddEnrollment(studentId, classId, DateTime.Now);

        }

        string GenerateClassCode(string name, DateTime semester, int professorId)
        {
            {
                string inputString = $"{name}{semester.ToString("yyyyMMdd")}{professorId}";
                int hashCode = inputString.GetHashCode();
                hashCode = Math.Abs(hashCode);
                string classCode = hashCode.ToString("D8");
                return classCode;
            }
        }


    }
}

