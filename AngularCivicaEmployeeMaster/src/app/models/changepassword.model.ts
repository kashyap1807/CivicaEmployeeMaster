export interface ChangePassword{
    loginId: string|null|undefined,
    oldPassword: string,
    newPassword: string,
    confirmNewPassword: string,
}