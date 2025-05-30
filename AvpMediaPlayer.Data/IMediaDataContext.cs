﻿using AvpMediaPlayer.Data.Models;

namespace AvpMediaPlayer.Data
{
    public interface IMediaDataContext
    {
        MediaList Add(string listName, string[] media);
        MediaList[] Get();
        MediaList? Get(string listName);
        void Delete(string listName);
        void Remove(string listName, string[] media);
        void Rename(string listName, string newName);
        void Clear(string listName);
    }
}