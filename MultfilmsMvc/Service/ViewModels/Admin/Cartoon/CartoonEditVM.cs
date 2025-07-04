using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.ViewModels.Admin.Cartoon
{
    public class CartoonEditVM
    {
        public string Name { get; set; }
        public string ExistImage { get; set; }
        public IFormFile? UploadImage { get; set; }
        public string Plot { get; set; }
        public int Year { get; set; }
        public string PlayerUrl { get; set; }
        public int? SelectedCategoryId { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<int> SelectedDirectorIds { get; set; } = new();
        public List<SelectListItem> DirectorOptions { get; set; } = new();
        public List<int> SelectedActorIds { get; set; } = new();
        public List<SelectListItem> ActorOptions { get; set; } = new();
        public List<int> SelectedGenreIds { get; set; } = new();
        public List<SelectListItem> GenreOptions { get; set; } = new();
        public List<int> SelectedStudioIds { get; set; } = new();
        public List<SelectListItem> StudioOptions { get; set; } = new();
        public List<int> SelectedCollectionIds { get; set; } = new();
        public List<SelectListItem> CollectionOptions { get; set; } = new();
        public List<int> SelectedCountryIds { get; set; } = new();
        public List<SelectListItem> CountryOptions { get; set; } = new();
    }
    public class CartoonEditVMValidator : AbstractValidator<CartoonEditVM>
    {
        public CartoonEditVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название обязательно")
                .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

            RuleFor(x => x.Plot)
                .NotEmpty().WithMessage("Сюжет обязателен")
                .MaximumLength(1000).WithMessage("Сюжет не должен превышать 1000 символов");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage($"Год должен быть между 1900 и {DateTime.Now.Year + 1}");

            RuleFor(x => x.PlayerUrl)
                .NotEmpty().WithMessage("Ссылка на плеер обязательна")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("Ссылка должна быть валидным URL");

            RuleFor(x => x.SelectedCategoryId)
                .NotNull().WithMessage("Категория обязательна");

            RuleFor(x => x.SelectedDirectorIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы одного режиссёра");

            RuleFor(x => x.SelectedActorIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы одного актёра");

            RuleFor(x => x.SelectedGenreIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы один жанр");

            RuleFor(x => x.SelectedStudioIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы одну студию");

            RuleFor(x => x.SelectedCollectionIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы одну коллекцию");

            RuleFor(x => x.SelectedCountryIds)
                .Must(x => x != null && x.Any()).WithMessage("Выберите хотя бы одну страну");
        }
    }

}

