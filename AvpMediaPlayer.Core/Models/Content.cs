﻿namespace AvpMediaPlayer.Core.Models
{
    public abstract class Content
    {
        protected Content(string url)
        {
            Url = url;
        }

        public virtual string Name => Url;
        public virtual string RootPath => Url;
        public virtual bool IsDirectory => true;
        internal protected IList<Content> Contents { get; protected set; } = [];
        public string Url { get; }
        public string? ParentName { get => Path.GetDirectoryName(Url)?.Split(@"\").Last(); }
    }
}
