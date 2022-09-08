export enum UserRoles {
  superAdmin = "superAdmin",
  admin = "admin",
  nonAdmin = "nonAdmin",
}
//some views will be for admins only, some for users (non-admins)
// and then the rest is available for all roles
export const userRoles = {
  admins: [String(UserRoles.superAdmin), String(UserRoles.admin)],
  users: [String(UserRoles.nonAdmin)],
  all: [
    String(UserRoles.superAdmin),
    String(UserRoles.admin),
    String(UserRoles.nonAdmin),
  ],
};
