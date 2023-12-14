using System;
using Microsoft.EntityFrameworkCore;

namespace ClassNegarService.Db
{
    public class ClassNegarDbContext : DbContext
    {
        public ClassNegarDbContext(DbContextOptions<ClassNegarDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassNotification> ClassNotifications { get; set; }
        public DbSet<ClassResouce> ClassResouces { get; set; }
        public DbSet<ClassTime> ClassTimes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<NotificationComment> NotificationComments { get; set; }
        public DbSet<NotificationDislike> NotificationDislikes { get; set; }
        public DbSet<NotificationLike> NotificationLikes { get; set; }
        public DbSet<ProfessorAttendance> ProfessorAttendances { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }


    }
}

