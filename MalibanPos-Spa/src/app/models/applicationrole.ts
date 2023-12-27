import { ApplicationUserRole } from "./applicationuserrole";
import { RolePermission } from "./rolepermission";

export interface ApplicationRole  {
  roleName: string;
  roleIcon: string;
  isActive: boolean;
  permissions: RolePermission[];
  description: string;
  createdDate: string;
  applicationUserRoles: ApplicationUserRole[];
}