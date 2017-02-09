using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization.Services;
using Orchard.Localization.Models;
using System.Collections.Generic;
using System.Linq;

public class MyContentHandler : ContentHandler
{
    readonly IOrchardServices orchardServices;
    private readonly ICultureManager _cultureManager;
    private readonly IContentManager _contentManager;

    public MyContentHandler(IRepository<LocalizationPartRecord> localizedRepository, ICultureManager cultureManager, IContentManager contentManager, IOrchardServices orchardServices)
    {
        this.orchardServices = orchardServices;
        _cultureManager = cultureManager;
        _contentManager = contentManager;
    }

    protected override void BuildDisplayShape(BuildDisplayContext context)
    {
        if (context.DisplayType == "Detail" && ((IShape)context.Shape).Metadata.Type == "Content" &&
            orchardServices.WorkContext.GetState<ContentItem>("currentContentItem") == null &&
            context.ContentItem.ContentType == "Page")
        {
            ILocalizationService service = new LocalizationService(_contentManager, _cultureManager);
            IEnumerable<LocalizationPart> Localizations = service.GetLocalizations(context.Content, VersionOptions.Published);
            orchardServices.WorkContext.SetState("currentContentLocalizations", Localizations);
            orchardServices.WorkContext.SetState("currentContentItem", context.ContentItem);
        }
    }
}