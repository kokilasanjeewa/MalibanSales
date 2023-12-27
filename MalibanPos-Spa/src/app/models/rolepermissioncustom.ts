export interface RolePermissionCustom {
  id: number;
  allow: boolean;
  mainMenu: string;
  submenu: string;
  submenuItem: string;
  accessControllerName: string;
  applicationRoleId: number;
  applicationUserId: number;
  userName: string;
  roleName: string;
}
