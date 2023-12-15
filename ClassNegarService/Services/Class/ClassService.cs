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
            await _classRepo.AddClass(model, code, password, professorId);

        }

        public Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId)
        {
            throw new NotImplementedException();
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

