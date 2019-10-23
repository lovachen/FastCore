﻿using System;
using cts.web.core;
using cts.web.core.MediaItem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using cts.web.core.Model;
using Microsoft.AspNetCore.Http;
using cts.web.core.Mail;
using AspNetCore.Cache;
using AutoMapper;
using Fast.Mapping;
using Fast.Entities;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Fast.Framework.Security;
using Microsoft.AspNetCore.Builder;
using cts.web.core.Librs; 

namespace Fast.Framework
{
    public class FastEngine : Engine, IEngine
    {
        private readonly IWebHostEnvironment _hosting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public FastEngine(IConfiguration configuration, IWebHostEnvironment hosting) : base(configuration)
        {
            _hosting = hosting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public override void Initialize(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddDbContextPool<ABDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),100);
            services.AddDbContext<FastDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //程序集依赖注入
            services.AddAssembly("Fast.Services");

            //ApiController 的模型验证错误返回
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var res = context.ModelState.Where(e => e.Value.Errors.Any())
                    .Select(e => new ApiJsonResult()
                    {
                        code = 1005,
                        msg = e.Value.Errors.First().ErrorMessage
                    }).FirstOrDefault();
                    return new OkObjectResult(res);
                };
            });
            services.AddSingleton<IWebHelper, WebHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMailProvide, MailProvide>();
            services.AddSingleton<IMediaItemStorage, MediaItemStorage>();
            services.AddScoped<SiteWorkContext>();
            services.AddScoped<SysUserAuthentication>();
            services.AddScoped<WorkContext>();

            //启用redis或者内存缓存,默认使用内存缓存
            services.AddRedisOrMemoryCache(Configuration);
            //
            //var cfg = new MapperConfiguration(cfg=>cfg.AddProfile<MappingProfile>());
            // cfg.AssertConfigurationIsValid();
            //services.AddAutoMapper(RuntimeHelper.GetAssemblyByName("Fast.Entities"), RuntimeHelper.GetAssemblyByName("Fast.Mapping"));
            services.AddAutoMapper(typeof(MappingProfile));

            //启用JWT
            services.AddJwt(_hosting);

            //API版本
            services.AddApiVersioning(opts =>
            { 
                opts.AssumeDefaultVersionWhenUnspecified = true;
            });

            //中文编码 https://docs.microsoft.com/zh-cn/aspnet/core/security/cross-site-scripting?view=aspnetcore-2.1#customizing-the-encoders
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));

            //Cookie登陆状态保存设置
            services.AddAuthentication(o =>
            {
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
                {
                    opts.Cookie.HttpOnly = true;
                    opts.LoginPath = "/admin";
                });

        }
    }
}
