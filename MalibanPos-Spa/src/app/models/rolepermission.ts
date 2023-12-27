import { ApplicationRole } from "./applicationrole";
import { ApplicationUser } from "./applicationuser";
import { MainMenu } from "./mainmenu";
import { Submenu } from "./submenu";

export interface RolePermission {
  id: number;
  allow: boolean;
  mainMenuId: number;
  submenuId: number;
  mainMenu: MainMenu;
  submenu: Submenu;
  accessControllerName: string;
  applicationRoleId: number;
  applicationRole: ApplicationRole;
  applicationUserId: number;
  applicationUser: ApplicationUser;
}