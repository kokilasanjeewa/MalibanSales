import { EmailValidator } from '@angular/forms';

export class SignUp {
  constructor(
    public firstName: string,
    public lastName: string,
    public email: EmailValidator,
    public password: string,
    public confirmPassword: string,
    public branch: []
  ) {}
}
