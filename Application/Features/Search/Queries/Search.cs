using FluentValidation;
using MediatR;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using Application.Features.Search.Dtos;
using Application.Contexts;

namespace Application.Features.DictionaryKeywords.Comands
{
    /// <summary>
    /// Giving a keyword or part of it, search the keyword definitions on the dictionary
    /// </summary>
    public static class Search
    {
        /// <summary>
        /// The class that defines the query. Additional query parameters would be added here.
        /// </summary>
        public class SearchKeyword : IRequest<KeywordSearchResponseDto>
        {
            public string Keyword { get; set; }
        }

       
        public class SearchKeywordValidator : AbstractValidator<SearchKeyword>
        {
            protected ApplicationContext Context { get; }
            public SearchKeywordValidator(IServiceProvider serviceProvider)
            {
                Context = serviceProvider.GetService<ApplicationContext>();
                RuleFor(user => user.Keyword)
                    .NotEmpty();
                RuleFor(c => c).CustomAsync(async (command, context, cancellationToken) =>
                {
                    var isAvailable = await Context.DictionaryKeywords.Where(c=>c.Keyword.Contains(command.Keyword)).AnyAsync();

                    if (!isAvailable)
                    {
                        context.AddFailure(nameof(command.Keyword), $"No entries found on our dictionary for {command.Keyword}.");
                    }
                });
            }
        }

        /// <summary>
        /// When this query is executed, this class will generate the result
        /// </summary>
        public class QueryHandler : IRequestHandler<SearchKeyword, KeywordSearchResponseDto>
        {
            private readonly ApplicationContext _context;
            private readonly IDictionaryService _dictionary;

            public QueryHandler(
                ApplicationContext context,
                IDictionaryService dictionary)
            {
                _context = context;
                _dictionary = dictionary;
            }

            /// <summary>
            /// Search keyword definition
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public virtual async Task<KeywordSearchResponseDto> Handle(SearchKeyword request, CancellationToken cancellationToken)
            {
                var Result = new KeywordSearchResponseDto(new List<KeywordDefinitionsDto>());
                //check if the requested keyword exists among the databse dictionary keywords
                var keywords = await _context.DictionaryKeywords.Where(c => c.Keyword.Contains(request.Keyword)).ToListAsync();
                
                //for each found keyword get the definition from dictionary
                foreach (var keyword in keywords)
                {
                    var itemDefinition = await _dictionary.GetKeywordDefinitionAsync(keyword.Keyword, cancellationToken);
                    Result.Add(itemDefinition);
                }
                //return the result
                return Result;  
            }
        }
    }

}
