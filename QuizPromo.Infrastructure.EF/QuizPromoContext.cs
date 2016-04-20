using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore;

namespace QuizPromo.Infrastructure.EF
{
    public class QuizPromoContext : DbContext
    {
        public QuizPromoContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Server=(localDb)\v11.0;Database=QuizPromo;Trusted_Connection=True;Integrated Security=True");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var categoryMap = modelBuilder.Entity<Category>();
            categoryMap
                .HasIncrementKey();
            categoryMap
                .Property(e => e.Name)
                .IsRequired();

            var questionMap = modelBuilder.Entity<Question>();
            questionMap
                .HasIncrementKey();

            questionMap
                .Property(e => e.Statement)
                .IsRequired();

            questionMap
                .HasOne(e => e.Category)
                .WithMany()
                .IsRequired();

            questionMap
                .AlwaysIncludeForeignEntity(e => e.Category);

            var choiceQuestionMap = modelBuilder.Entity<ChoiceQuestion>();
            choiceQuestionMap
                .HasMany(e => e.AnswerVariants)
                .WithOne(e => e.Question)
                .HasForeignKey("QuestionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            var mcMap = modelBuilder.Entity<SingleChoiceQuestion>();
            var scMap = modelBuilder.Entity<MultipleChoicesQuestion>();

            var answerVariant = modelBuilder.Entity<AnswerVariant>();
            answerVariant
                .HasIncrementKey();

            var userMap = modelBuilder.Entity<User>();
            userMap.HasIncrementKey();
            userMap
                .Property(e => e.FullName)
                .IsRequired();

            var testMap = modelBuilder.Entity<Test>();
            testMap.HasIncrementKey();
            testMap
                .HasMany(e => e.Questions);
            testMap.Property(e => e.Title).HasMaxLength(100);
            testMap.Property(e => e.Description).HasMaxLength(4000);


            var sessionMap = modelBuilder.Entity<Session>();
            sessionMap.HasIncrementKey();
            sessionMap
                .HasMany(e => e.AssignedQuestions)
                .WithOne(e => e.Session)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            sessionMap
                .HasMany(e => e.Answers)
                .WithOne(e => e.Session)
                .OnDelete(DeleteBehavior.Cascade);
            sessionMap
                .HasOne(e => e.Test)
                .WithMany()
                .IsRequired();
            sessionMap
                .HasOne(e => e.User)
                .WithMany()
                .IsRequired();
            sessionMap
                .HasOne(e => e.CurrentQuestion)
                .WithMany();


            var sessionQuestionMap = modelBuilder.Entity<SessionQuestion>();
            sessionQuestionMap
                .HasKey(e => new { e.QuestionId, e.SessionId });
            sessionQuestionMap
                .HasOne(e => e.Question)
                .WithMany()
                .HasForeignKey(e => e.QuestionId);


            //answerResultMap
            //    .AlwaysIncludeForeignEntity(e => e.Answer);

            var answerMap = modelBuilder.Entity<Answer>();
            answerMap
                .HasOne(e => e.Question)
                .WithMany();

            var singleAnswerMap = modelBuilder.Entity<SingleAnswer>();


            var multipleAnswerMap = modelBuilder.Entity<MultipleAnswer>();
            multipleAnswerMap.HasMany(e => e.Choices)
                .WithOne(e => e.MultipleAnswer)
                .HasForeignKey(e => e.MultipleAnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            var singleAnswerInMultipleAnswerMap = modelBuilder.Entity<SingleAnswerInMultipleAnswer>();
            singleAnswerInMultipleAnswerMap
                .HasKey(e => new { e.SingleAnswerId, e.MultipleAnswerId });
            singleAnswerInMultipleAnswerMap
                .HasOne(e => e.SingleAnswer)
                .WithMany()
                .HasForeignKey(e => e.SingleAnswerId)
                .OnDelete(DeleteBehavior.Restrict);
        }



        // todo deactivate instead of deleting
    }



    public static class EntityTypeBuilderExtensions
    {
        private const string ForeignDictionariesMetadataMarker = "ForeignDictionaries";

        public static void HasIncrementKey<T>(this EntityTypeBuilder<T> builder) where T : Entity<int>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseSqlServerIdentityColumn();
        }

        public static EntityTypeBuilder<TEntity> AlwaysIncludeForeignEntity<TEntity, TRelatedEntity>(
            this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TRelatedEntity>> includedProperty) where TEntity : class where TRelatedEntity : class
        {
            // todo check valid lambda that can fit to TEntity
            builder.Metadata[ForeignDictionariesMetadataMarker] = new List<LambdaExpression>() { includedProperty };

            return builder;
        }

        public static ICollection<LambdaExpression> GetIncludedForeignEntities<TEntity>(this IModel model)
        {
            var collection = model.FindEntityType(typeof(TEntity))[ForeignDictionariesMetadataMarker] as ICollection<LambdaExpression>;

            return collection ?? Enumerable.Empty<LambdaExpression>().ToList();
        }
    }
}
