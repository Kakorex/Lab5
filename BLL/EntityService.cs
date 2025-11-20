using Abstraction;
using BLL;
using Common;
using System;

namespace BLL
{
    public abstract class EntityService<TEntity, TDto> where TEntity : Person
        where TDto : BLL.PersonDto
    {
        protected readonly IEntityContext<TEntity> _context;

        protected EntityService(IEntityContext<TEntity> context)
        {
            _context = context;
        }

        protected abstract TEntity MapToEntity(TDto dto);
        protected abstract TDto MapToDto(TEntity entity);

        public void Add(TDto dto)
        {
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName))
            {
                throw new BusinessLogicException("First name and last name must be specified.");
            }
            try
            {
                TEntity entity = MapToEntity(dto);
                _context.Add(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException("Error adding entity to data layer.", ex);
            }
        }

        public List<TDto> GetAll()
        {
            try
            {
                return _context.GetAll().Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException("Error retrieving entities from data layer.", ex);
            }
        }

        public TDto Find(string firstName, string lastName, string passport)
        {
            try
            {
                TEntity entity = _context.GetAll()
                    .FirstOrDefault(e =>
                        e.FirstName == firstName &&
                        e.LastName == lastName &&
                        e.Passport == passport);

                if (entity == null)
                {
                    throw new BusinessLogicException("Person not found.");
                }

                return MapToDto(entity);
            }
            catch (Exception ex) when (!(ex is BusinessLogicException))
            {
                throw new BusinessLogicException("Error searching for person.", ex);
            }
        }

        public void Remove(string firstName, string lastName, string passport)
        {
            try
            {
                List<TEntity> entities = _context.GetAll();
                TEntity entityToRemove = entities.FirstOrDefault(e =>
                    e.FirstName == firstName &&
                    e.LastName == lastName &&
                    e.Passport == passport);

                if (entityToRemove == null)
                {
                    throw new BusinessLogicException("Person not found for removal.");
                }

                entities.Remove(entityToRemove);
                _context.SaveAll(entities);
            }
            catch (Exception ex) when (!(ex is BusinessLogicException))
            {
                throw new BusinessLogicException("Error removing person.", ex);
            }
        }
    }

    public class StudentService : EntityService<Student, StudentDto>
    {
        public StudentService(IEntityContext<Student> context)
            : base(context) { }

        protected override Student MapToEntity(StudentDto dto)
        {
            return new Student(dto.FirstName, dto.LastName, dto.Passport, dto.StudentID, dto.Course, dto.MilitaryID);
        }

        protected override StudentDto MapToDto(Student entity)
        {
            return new StudentDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                StudentID = entity.StudentID,
                Course = entity.Course,
                MilitaryID = entity.MilitaryID
            };
        }

        public List<StudentDto> FindFifthYearStudentsWheServed()
        {
            List<StudentDto> allStudents = GetAll();

            return allStudents
                .Where(s => s.Course == 5 && !string.IsNullOrEmpty(s.MilitaryID) && s.MilitaryID != "N/A")
                .ToList();
        }
        public string RecitePoems(StudentDto dto)
        {
            return $"Студент {dto.FirstName} {dto.LastName} на {dto.Course} курсі декламує: 'Освіта — це світло, а неуцтво — тьма. Вперед до знань!'";
        }
    }

    public class FootballPlayerService : EntityService<FootballPlayer, FootballPlayerDto>
    {
        public FootballPlayerService(IEntityContext<FootballPlayer> context)
            : base(context) { }

        protected override FootballPlayer MapToEntity(FootballPlayerDto dto)
        {
            return new FootballPlayer(dto.FirstName, dto.LastName, dto.Passport, dto.Team);
        }

        protected override FootballPlayerDto MapToDto(FootballPlayer entity)
        {
            return new FootballPlayerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                Team = entity.Team
            };
        }
        public string Practice(FootballPlayerDto dto)
        {
            return $"{dto.FirstName} {dto.LastName} ({dto.Team}) тренує швидкість, точність пасів та удари по воротах.";
        }
    }

    public class LawyerService : EntityService<Lawyer, LawyerDto>
    {
        public LawyerService(IEntityContext<Lawyer> context)
            : base(context) { }

        protected override Lawyer MapToEntity(LawyerDto dto)
        {
            return new Lawyer(dto.FirstName, dto.LastName, dto.Passport, dto.Company);
        }

        protected override LawyerDto MapToDto(Lawyer entity)
        {
            return new LawyerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                Company = entity.Company
            };
        }
        public string Practice(LawyerDto dto)
        {
            return $"{dto.FirstName} {dto.LastName} з компанії '{dto.Company}' готує судові документи, аналізує прецеденти та веде переговори.";
        }
    }
}