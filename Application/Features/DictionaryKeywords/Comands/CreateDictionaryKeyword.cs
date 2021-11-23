using FluentValidation;
using MediatR;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Application.Contexts;
using Application.Features.DictionaryKeywords.Models;

namespace Application.Features.DictionaryKeywords.Comands
{
    /// <summary>
    /// add keyword to dictionary initial keywords
    /// </summary>
    public static class CreateDictionaryKeyword
    {
        /// <summary>
        /// The class that defines the query. Additional query parameters would be added here.
        /// </summary>
        public class AddDictionaryKeyword : IRequest<Result>
        {
            public string Keyword { get; set; }
        }

        /// <summary>
        /// The class that defines the response, or result.
        /// </summary>
        public class Result
        {
            public bool Sucess { get; set; }

            public string ErrorMessage { get; set; }
        }

        public class AddKeywordValidator : AbstractValidator<AddDictionaryKeyword>
        {
            protected ApplicationContext Context { get; }

            public AddKeywordValidator(IServiceProvider serviceProvider)
            {
                Context = serviceProvider.GetService<ApplicationContext>();
                RuleFor(user => user.Keyword)
                    .NotEmpty();
                RuleFor(c => c).CustomAsync(async (command, context, cancellationToken) =>
                {
                    var isUnique = await Context.DictionaryKeywords.AnyAsync(c=>c.Keyword == command.Keyword);

                    if (!isUnique)
                    {
                        context.AddFailure(nameof(command.Keyword), $"A Dictionary keyword {command.Keyword} already exists.");
                    }
                });
            }

        }

        /// <summary>
        /// When this query is executed, this class will generate the result
        /// </summary>
        public class QueryHandler : IRequestHandler<AddDictionaryKeyword, Result>
        {
            private readonly ApplicationContext _context;

            public QueryHandler(
                ApplicationContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Add Keyword
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public virtual async Task<Result> Handle(AddDictionaryKeyword request, CancellationToken cancellationToken)
            {
                var newDictionaryKeyword = new DictionaryKeyword
                {
                    Keyword = request.Keyword
                };

                await _context.AddAsync(newDictionaryKeyword);
                await _context.SaveChangesAsync();
                return new Result
                {
                    Sucess = true,
                };
            }
        }
    }

}
