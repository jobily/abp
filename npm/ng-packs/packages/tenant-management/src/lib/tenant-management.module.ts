import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TenantsComponent } from './components/tenants/tenants.component';
import { TenantManagementExtensionsGuard } from './guards/extensions.guard';
import { TenantManagementConfigOptions } from './models/config-options';
import { TenantManagementRoutingModule } from './tenant-management-routing.module';
import {
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import { PageModule } from '@abp/ng.components/page';
import { FeatureManagementComponent } from '@abp/ng.feature-management';

@NgModule({
  declarations: [TenantsComponent],
  exports: [TenantsComponent],
  imports: [
    TenantManagementRoutingModule,
    NgxValidateCoreModule,
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    ExtensibleModule,
    PageModule,
    FeatureManagementComponent,
  ],
})
export class TenantManagementModule {
  static forChild(
    options: TenantManagementConfigOptions = {},
  ): ModuleWithProviders<TenantManagementModule> {
    return {
      ngModule: TenantManagementModule,
      providers: [
        {
          provide: TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        TenantManagementExtensionsGuard,
      ],
    };
  }

  static forLazy(
    options: TenantManagementConfigOptions = {},
  ): NgModuleFactory<TenantManagementModule> {
    return new LazyModuleFactory(TenantManagementModule.forChild(options));
  }
}
