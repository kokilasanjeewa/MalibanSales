using System;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
    public class SessionGoodReceptNote : GoodReceptNote
    {
        public static GoodReceptNote GetGoodReceptNote(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionGoodReceptNote goodReceptNote = session?.GetJson<SessionGoodReceptNote>("GoodReceptNote") ?? new SessionGoodReceptNote();
            goodReceptNote.Session = session;
            return goodReceptNote;
        }
        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            base.AddItem(goodReceptNoteDetail);
            Session.SetJson("GoodReceptNote", this);
        }
        public override void UpdateQuantity(GoodReceptNoteDetail goodReceptNoteDetail, int quantity)
        {
            base.UpdateQuantity(goodReceptNoteDetail, quantity);
            Session.SetJson("GoodReceptNote", this);
        }
          public override void RemoveLine(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            base.RemoveLine(goodReceptNoteDetail);
            Session.SetJson("GoodReceptNote", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session.Remove("GoodReceptNote");
        }

    }
}