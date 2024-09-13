export interface User{
    salutation: string,
    name: string,
    age: number|null,
    dateOfBirth: string|null,
    loginId: string,
    gender: string,
    email: string,
    phone: number,
    password: string,
    confirmPassword: string,
    passwordHintId: number,
    passwordHintAnswer: string,
  }