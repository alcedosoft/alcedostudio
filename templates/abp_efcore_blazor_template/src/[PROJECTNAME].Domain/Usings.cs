global using IdentityServer4.Models;
global using Volo.Abp.AuditLogging;
global using Volo.Abp.Authorization.Permissions;
global using Volo.Abp.BackgroundJobs;
global using Volo.Abp.Data;
global using Volo.Abp.DependencyInjection;
global using Volo.Abp.Domain.Entities.Auditing;
global using Volo.Abp.Domain.Repositories;
global using Volo.Abp.Emailing;
global using Volo.Abp.FeatureManagement;
global using Volo.Abp.Guids;
global using Volo.Abp.Identity;
global using Volo.Abp.IdentityServer;
global using Volo.Abp.IdentityServer.ApiResources;
global using Volo.Abp.IdentityServer.ApiScopes;
global using Volo.Abp.IdentityServer.Clients;
global using Volo.Abp.IdentityServer.IdentityResources;
global using Volo.Abp.Modularity;
global using Volo.Abp.MultiTenancy;
global using Volo.Abp.PermissionManagement;
global using Volo.Abp.PermissionManagement.Identity;
global using Volo.Abp.PermissionManagement.IdentityServer;
global using Volo.Abp.SettingManagement;
global using Volo.Abp.Settings;
global using Volo.Abp.TenantManagement;
global using Volo.Abp.Uow;
global using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
global using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
global using Client = Volo.Abp.IdentityServer.Clients.Client;

[assembly: InternalsVisibleToAttribute("[PROJECTNAME].Domain.Tests")]
[assembly: InternalsVisibleToAttribute("[PROJECTNAME].TestBase")]
