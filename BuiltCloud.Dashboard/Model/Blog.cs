using Built.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BuiltCloud.BlogModel
{
    /// <summary>
    /// 博客
    /// </summary>
    [ConnectionName("BlogCloud")]
    public class Blog : Entity
    {
        /// <summary>
        /// 目录
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 编辑
        /// </summary>
        public string Editer { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// 特色（置顶，原创，精品）
        /// </summary>
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// 原文链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }
    }

    /*
     博客表
     标题
     原创作者
     编辑者
     分类
     tag
     时间
     内容
     特色（置顶，原创，精品）
     原文链接
     版本
     userid
     修改时间

     log

     评论表
     收藏表
     Tag表
     分类表CATEGORIES
     用户表
      */
}