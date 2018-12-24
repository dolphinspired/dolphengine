using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphEngine.Demo
{
    public static class ContentManagerExtensions
    {
        public static ContentManager LoadAll<T>(this ContentManager content, params string[] files)
        {
            foreach (var file in files)
            {
                content.Load<T>(file);
            }

            return content;
        }

        public static async Task<ContentManager> LoadAllAsync<T>(this ContentManager content, params string[] files)
        {
            var tasks = files.Select(file => new Task(() => content.Load<T>(file)));

            await Task.WhenAll(tasks);

            return content;
        }
    }
}
