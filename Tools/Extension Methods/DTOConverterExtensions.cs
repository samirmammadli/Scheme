using Scheme.DTO;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.OutputDataConvert
{
    public static class DTOConverterExtensions
    {
        public static ProjectOutput GetDTO(this Project project, ProjectUserRole? role = null)
        {
            if (project == null)
                return null;

            var projectOutput = new ProjectOutput
            {
                Id = project.Id,
                CreationDate = project.CreationDate,
                Name = project.Name,
                Role = role == null ? project.Roles.FirstOrDefault().Type : (ProjectUserRole)role
            };

            return projectOutput;
        }

        public static IEnumerable<ProjectOutput> GetDTO(this IEnumerable<Project> projects)
        {
            if (projects == null)
                return null;

            var projectsOutput = new List<ProjectOutput>();

            foreach (var item in projects)
            {
                if (item != null)
                    projectsOutput.Add(GetDTO(item));
            }

            return projectsOutput;
        }

        public static BacklogOutput GetDTO(this Backlog backlog)
        {
            if (backlog == null)
                return null;

            var backlogOutput = new BacklogOutput
            {
                Id = backlog.Id,
                ColumnType = backlog.ColumnType,
                Name = backlog.Name
            };

            return backlogOutput;
        }

        public static SprintOutput GetDTO(this Sprint sprint)
        {
            if (sprint == null)
                return null;

            var backlogOutput = new SprintOutput
            {
                Id = sprint.Id,
                Name = sprint.Name,
                ExpireDate = sprint.ExpireDate
            };

            return backlogOutput;
        }

        public static IEnumerable<SprintOutput> GetDTO(this IEnumerable<Sprint> sprints)
        {
            if (sprints == null)
                return null;

            var sprintsOutput = new List<SprintOutput>();

            foreach (var item in sprints)
            {
                if (item != null)
                    sprintsOutput.Add(GetDTO(item));
            }

            return sprintsOutput;
        }

        public static ColumnOutput GetDTO(this Column column)
        {
            if (column == null)
                return null;

            var columnOutput = new ColumnOutput
            {
                Id = column.Id,
                Name = column.Name,
                ProjectId = column.Project.Id
            };

            return columnOutput;
        }

        public static IEnumerable<ColumnOutput> GetDTO(this IEnumerable<Column> columns)
        {
            if (columns == null)
                return null;

            var columnsOutput = new List<ColumnOutput>();

            foreach (var item in columns)
            {
                if (item != null)
                    columnsOutput.Add(GetDTO(item));
            }

            return columnsOutput;
        }

        public static CardOutput GetDTO(this Card card)
        {
            if (card == null)
                return null;

            var columnOutput = new CardOutput
            {
                Id = card.Id,
                Text = card.Text,
                ProjectId = card.Column.Project.Id,
                SprintId = card.Column.Sprint.Id
            };

            return columnOutput;
        }

        public static IEnumerable<CardOutput> GetDTO(this IEnumerable<Card> cards)
        {
            if (cards == null)
                return null;

            var cardsOutput = new List<CardOutput>();

            foreach (var item in cards)
            {
                if (item != null)
                    cardsOutput.Add(GetDTO(item));
            }

            return cardsOutput;
        }
    }
}
