using Scheme.DTO;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.OutputDataConvert
{
    public static class DTOConverterExtension
    {
        public static ProjectOutput GetDTO(this Project project, ProjectUserRole role)
        {
            if (project == null)
                return null;

            var projectOutput = new ProjectOutput
            {
                Id = project.Id,
                CreationDate = project.CreationDate,
                Name = project.Name,
                Role = role
            };

            return projectOutput;
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
                Name = column.Name
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
    }
}
