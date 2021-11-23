using System;
using Application.Interfaces;
using System.Threading.Tasks;
using Application.Features.Search.Dtos;
using System.Threading;
using RestSharp;
using AutoMapper;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Search.Services
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FreeDictionaryResponse, KeywordDefinitionsDto>().ConvertUsing(new FreeDictionaryResponseToDtoConverter());

        }
    }

    internal class FreeDictionaryResponseToDtoConverter : ITypeConverter<FreeDictionaryResponse, KeywordDefinitionsDto>
    {
        public KeywordDefinitionsDto Convert(FreeDictionaryResponse source, KeywordDefinitionsDto destination, ResolutionContext context)
        {
            var response = new KeywordDefinitionsDto
            {
                Definitions = new System.Collections.Generic.List<KeywordSearchDefinitionDto>()
            };

            if (source.Any() != true)
            {
                return response;
            }

            var wordDefinitions = source.FirstOrDefault();
            response.Word = wordDefinitions.Word;
            foreach (var definition in wordDefinitions.Meanings)
            {
                var wordDefinition = new KeywordSearchDefinitionDto
                {
                    Definition = definition.Definitions?.FirstOrDefault()?.Definition,
                    PartOfSpeach = definition.PartOfSpeech
                };
                response.Definitions.Add(wordDefinition);
            }

            return response;
        }
    }


    /// <summary>
    /// The Free Dictionary API implementation
    /// </summary>
    public class FreeDictionaryService : IDictionaryService
    {
        private readonly IRestClient _restClient;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public FreeDictionaryService(IRestClient restClient, IMapper mapper, IMemoryCache memoryCache)
        {
            _restClient = restClient;
            _restClient.BaseUrl = new Uri("https://api.dictionaryapi.dev/api/v2/entries/en/");
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<KeywordDefinitionsDto> GetKeywordDefinitionAsync(string keyword, CancellationToken cancellationToken)
        {
            // get the word definition from cache, if not found get it from the API
            return await _memoryCache.GetOrCreateAsync<KeywordDefinitionsDto>(GetCacheKey(keyword), async entry =>
             {
                 var definitions = await GetKeywordDefinitions(keyword, cancellationToken);
                 return _mapper.Map<KeywordDefinitionsDto>(definitions);
             });
        }

        private async Task<FreeDictionaryResponse> GetKeywordDefinitions(string keyword, CancellationToken cancellationToken)
        {
            var request = new RestRequest(keyword, Method.GET);
            var response = await _restClient.GetAsync<FreeDictionaryResponse>(request, cancellationToken);
            return response;
        }

        private string GetCacheKey(string keyword) => $"FREE_DICTIONARY_DEFINITION_${keyword}";

    }
}
