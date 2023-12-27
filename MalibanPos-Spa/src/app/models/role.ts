import { Permission } from "./permission";

export interface Role {
  roleName: string;
  permission: Permission[];
}