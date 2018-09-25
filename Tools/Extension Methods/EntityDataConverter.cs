using Scheme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.OutputDataConvert
{
    public static class EntityDataConverter
    {
        public static ProjectOutput AdaptForOutput(this Project project)
        {
            if (project == null)
                return null;

            var projectOutput = new ProjectOutput
            {
                Id = project.Id,
                CreationDate = project.CreationDate,
                Name = project.Name
            };

            return projectOutput;
        }

        public static IEnumerable<ProjectOutput> AdaptForOutput(this IEnumerable<Project> projects)
        {
            if (projects == null)
                return null;

            var projectsOutput = new List<ProjectOutput>();

            foreach (var item in projects)
            {
                if (item != null)
                    projectsOutput.Add(AdaptForOutput(item));
            }

            return projectsOutput;
        }

        public static BacklogOutput AdaptForOutput(this Backlog backlog)
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

        public static IEnumerable<BacklogOutput> AdaptForOutput(this IEnumerable<Backlog> backlogs)
        {
            if (backlogs == null)
                return null;

            var backlogsOutput = new List<BacklogOutput>();

            foreach (var item in backlogs)
            {
                if (item != null)
                    backlogsOutput.Add(AdaptForOutput(item));
            }

            return backlogsOutput;
        }

        public static SprintOutput AdaptForOutput(this Sprint sprint)
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

        public static IEnumerable<SprintOutput> AdaptForOutput(this IEnumerable<Sprint> sprints)
        {
            if (sprints == null)
                return null;

            var sprintsOutput = new List<SprintOutput>();

            foreach (var item in sprints)
            {
                if (item != null)
                    sprintsOutput.Add(AdaptForOutput(item));
            }

            return sprintsOutput;
        }
    }
}
