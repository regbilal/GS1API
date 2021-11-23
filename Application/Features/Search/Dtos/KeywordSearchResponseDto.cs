using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Search.Dtos
{
    public class KeywordSearchResponseDto : List<KeywordDefinitionsDto>
    {
        public KeywordSearchResponseDto(List<KeywordDefinitionsDto> list)
        {
            AddRange(list);
        }
    }

    public class KeywordDefinitionsDto
    {
        public string Word { get; set; }
        public List<KeywordSearchDefinitionDto> Definitions { get; set; }
    }

    public class KeywordSearchDefinitionDto
    {
        public string PartOfSpeach { get; set; }
        public string Definition { get; set; }
    }
}
