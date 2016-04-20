using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using QuizPromo.Infrastructure.EF;

namespace QuizPromo.Infrastructure.EF.Migrations
{
    [DbContext(typeof(QuizPromoContext))]
    partial class QuizPromoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QuizPromo.ModelCore.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsCorrect");

                    b.Property<int?>("QuestionId");

                    b.Property<int?>("SessionId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:DiscriminatorProperty", "Discriminator");

                    b.HasAnnotation("Relational:DiscriminatorValue", "Answer");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.AnswerVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("QuestionId")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Statement")
                        .IsRequired();

                    b.Property<int?>("TestId");

                    b.Property<DateTimeOffset>("Updated");

                    b.HasKey("Id");

                    b.HasAnnotation("fsfds", 1);

                    b.HasAnnotation("Relational:DiscriminatorProperty", "Discriminator");

                    b.HasAnnotation("Relational:DiscriminatorValue", "Question");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("BeginDate");

                    b.Property<int?>("CurrentQuestionId");

                    b.Property<DateTimeOffset>("EndDate");

                    b.Property<bool>("IsCompleted");

                    b.Property<int?>("TestId")
                        .IsRequired();

                    b.Property<int?>("UserId")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SessionQuestion", b =>
                {
                    b.Property<int>("QuestionId");

                    b.Property<int>("SessionId");

                    b.HasKey("QuestionId", "SessionId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleAnswerInMultipleAnswer", b =>
                {
                    b.Property<int>("SingleAnswerId");

                    b.Property<int>("MultipleAnswerId");

                    b.HasKey("SingleAnswerId", "MultipleAnswerId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.MultipleAnswer", b =>
                {
                    b.HasBaseType("QuizPromo.ModelCore.Answer");


                    b.HasAnnotation("Relational:DiscriminatorValue", "MultipleAnswer");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleAnswer", b =>
                {
                    b.HasBaseType("QuizPromo.ModelCore.Answer");

                    b.Property<string>("Choice");

                    b.HasAnnotation("Relational:DiscriminatorValue", "SingleAnswer");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.ChoiceQuestion", b =>
                {
                    b.HasBaseType("QuizPromo.ModelCore.Question");


                    b.HasAnnotation("Relational:DiscriminatorValue", "ChoiceQuestion");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.MultipleChoicesQuestion", b =>
                {
                    b.HasBaseType("QuizPromo.ModelCore.ChoiceQuestion");


                    b.HasAnnotation("Relational:DiscriminatorValue", "MultipleChoicesQuestion");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleChoiceQuestion", b =>
                {
                    b.HasBaseType("QuizPromo.ModelCore.ChoiceQuestion");


                    b.HasAnnotation("Relational:DiscriminatorValue", "SingleChoiceQuestion");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Answer", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.HasOne("QuizPromo.ModelCore.Session")
                        .WithMany()
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.AnswerVariant", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.ChoiceQuestion")
                        .WithMany()
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Question", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("QuizPromo.ModelCore.Test")
                        .WithMany()
                        .HasForeignKey("TestId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.Session", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.Question")
                        .WithMany()
                        .HasForeignKey("CurrentQuestionId");

                    b.HasOne("QuizPromo.ModelCore.Test")
                        .WithMany()
                        .HasForeignKey("TestId");

                    b.HasOne("QuizPromo.ModelCore.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SessionQuestion", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.HasOne("QuizPromo.ModelCore.Session")
                        .WithMany()
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleAnswerInMultipleAnswer", b =>
                {
                    b.HasOne("QuizPromo.ModelCore.MultipleAnswer")
                        .WithMany()
                        .HasForeignKey("MultipleAnswerId");

                    b.HasOne("QuizPromo.ModelCore.SingleAnswer")
                        .WithMany()
                        .HasForeignKey("SingleAnswerId");
                });

            modelBuilder.Entity("QuizPromo.ModelCore.MultipleAnswer", b =>
                {
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleAnswer", b =>
                {
                });

            modelBuilder.Entity("QuizPromo.ModelCore.ChoiceQuestion", b =>
                {
                });

            modelBuilder.Entity("QuizPromo.ModelCore.MultipleChoicesQuestion", b =>
                {
                });

            modelBuilder.Entity("QuizPromo.ModelCore.SingleChoiceQuestion", b =>
                {
                });
        }
    }
}
