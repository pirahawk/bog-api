using System;
using System.Collections.Generic;

namespace Bog.Api.Domain.Coordinators
{
    public interface IGetMediaLookupQuery
    {
        Dictionary<string, string> CreateMediaLookup(Guid articleId);
    }

    
}