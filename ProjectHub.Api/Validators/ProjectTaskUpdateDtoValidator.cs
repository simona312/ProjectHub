using FluentValidation;
using ProjectHub.Api.Dtos;


namespace ProjectHub.Api.Validators;

  
        public class ProjectTaskUpdateDtoValidator : AbstractValidator<ProjectTaskUpdateDto>
        {
            public ProjectTaskUpdateDtoValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required")
                    .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                    .MaximumLength(100).WithMessage("Title can be max 100 characters");

                RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Description));

                RuleFor(x => x.Status)
                    .IsInEnum()
                    .WithMessage("Invalid task status");

                RuleFor(x => x.Priority)
                    .IsInEnum()
                    .WithMessage("Invalid task priority");

                RuleFor(x => x.DueDate)
                    .Must(d => d == null || d.Value.Date <= DateTime.UtcNow.Date)
                    .WithMessage("DueDate cannot be in the past");
            }
        }

    




