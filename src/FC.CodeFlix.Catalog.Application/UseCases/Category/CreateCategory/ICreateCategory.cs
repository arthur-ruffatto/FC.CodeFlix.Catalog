﻿using FC.CodeFlix.Catalog.Application.UseCases.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
}