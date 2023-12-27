import { MainMenu } from "./mainmenu";

export interface Submenu {
  submenuId: number;
  name: string;
  mainMenuId: number;
  mainMenu: MainMenu;
}