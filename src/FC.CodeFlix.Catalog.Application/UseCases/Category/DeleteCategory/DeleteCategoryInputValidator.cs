﻿using FluentValidation;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInputValidator : AbstractValidator<DeleteCategoryInput>
{
    public DeleteCategoryInputValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.Id).NotEmpty();
    }
}