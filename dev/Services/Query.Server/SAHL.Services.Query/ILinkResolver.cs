using System;
using System.Collections;
using System.Collections.Generic;
using WebApi.Hal;

namespace SAHL.Services.Query
{
    public interface ILinkResolver
    {
        Link GetLink(string name);

        Link GetLink(Type representationType);

        Link GetLink(Type representationType, bool isSelf);
        Link GetLink(Type representationType, params object[] parameters);

        Link GetLink(Representation representation);
        Link GetLink(Representation representation, params object[] parameters);

        string GetHref(Type representationType);
        string GetHref(Type representationType, params object[] parameters);

        string GetHref(Representation representation);
        string GetHref(Representation representation, params object[] parameters);

        string GetRel(Type representationType);

        string GetRel(Representation representation);
    }
}
