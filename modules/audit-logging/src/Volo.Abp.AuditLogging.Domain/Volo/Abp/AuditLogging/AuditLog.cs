﻿using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class AuditLog : Entity<Guid>, IHasExtraProperties, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid? UserId { get; protected set; }

        public virtual Guid? ImpersonatorUserId { get; protected set; }

        public virtual Guid? ImpersonatorTenantId { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual string ClientIpAddress { get; protected set; }

        public virtual string ClientName { get; protected set; }

        public virtual string BrowserInfo { get; protected set; }

        public virtual string Exceptions { get; set; }

        public Dictionary<string, object> ExtraProperties { get; set; }

        public ICollection<EntityChange> EntityChanges { get; }

        public ICollection<AuditLogAction> Actions { get; protected set; }

        protected AuditLog()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public AuditLog(IGuidGenerator guidGenerator, AuditLogInfo auditInfo)
        {
            Id = guidGenerator.Create();
            TenantId = auditInfo.TenantId;
            UserId = auditInfo.UserId;
            ExecutionTime = auditInfo.ExecutionTime;
            ExecutionDuration = auditInfo.ExecutionDuration;
            ClientIpAddress = auditInfo.ClientIpAddress;
            ClientName = auditInfo.ClientName;
            BrowserInfo = auditInfo.BrowserInfo;
            ImpersonatorUserId = auditInfo.ImpersonatorUserId;
            ImpersonatorTenantId = auditInfo.ImpersonatorTenantId;
            ExtraProperties = auditInfo.ExtraProperties;
            EntityChanges = auditInfo.EntityChanges.Select(e => new EntityChange(guidGenerator, Id, e)).ToList();
            Actions = auditInfo.Actions.Select(e => new AuditLogAction(guidGenerator.Create(), Id, e)).ToList();
            Exceptions = String.Join(Environment.NewLine, auditInfo.Exceptions.Select(e=>e.ToString()).ToArray());
        }
    }
}
