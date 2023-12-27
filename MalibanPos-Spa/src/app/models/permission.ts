export interface Permission {
  id: number;
  Allow: boolean;
  deny: boolean;
  submenu: number;
  mainMenu: number;
  AccessControllerName: string;
}