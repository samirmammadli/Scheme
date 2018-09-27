using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms.Column;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Tools.Extension_Methods
{
    public static class CardContextExtensions
    {
        private static ControllerErrorCode _code;

        public static ControllerErrorCode GetError(this DbSet<Card> columns)
        {
            return _code;
        }

        public async static Task<IEnumerable<Card>> GetCards(this ProjectContext db, string userEmail, GetCardsForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.User == user && x.Project.Id == form.ProjectId);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var cards = await db.Cards.Where(x => x.Column.Id == form.ColumnId).ToListAsync();

            if (cards == null)
            {
                _code = ControllerErrorCode.CardNotFound;
                return null;
            }

            return cards;
        }

        public async static Task<Card> GetCard(this ProjectContext db, string userEmail, GetCardsForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.User == user && x.Project.Id == form.ProjectId);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var card = await db.Cards.FirstOrDefaultAsync(x => x.Column.Id == form.ColumnId);

            if (card == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return null;
            }

            return card;
        }

        public async static Task<bool> RemoveCard(this ProjectContext db, string userEmail, RemoveCardForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            var card = await db.Cards.FirstOrDefaultAsync(x => x.Column.Id == form.ProjectId && x.Id == form.CardId);

            if (card == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return false;
            }

            db.Cards.Remove(card);

            await db.SaveChangesAsync();

            return true;
        }

        public async static Task<Card> AddCard(this ProjectContext db, string userEmail, AddCardForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.Include(y => y.Project).FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var column = await db.Columns.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.ColumnId);

            if (column == null)
            {
                _code = ControllerErrorCode.SprintNotFound;
                return null;
            }


            var card = new Card
            {
                Column = column,
                Text = form.Name
            };

            await db.Cards.AddAsync(card);

            await db.SaveChangesAsync();

            return card;
        }

        public async static Task<bool> ChangeColumnName(this ProjectContext db, string userEmail, ChangeColumnNameForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var column = await db.Columns.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.ColumnId);

            if (column == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return false;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            column.Name = form.NewName;

            await db.SaveChangesAsync();

            return true;
        }
    }
}
