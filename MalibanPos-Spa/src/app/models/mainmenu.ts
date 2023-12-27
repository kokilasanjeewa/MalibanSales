import { RolePermission } from "./rolepermission";

export interface MainMenu {
  mainMenuId: number;
  name: string;
  rolePermissions: RolePermission[];
}