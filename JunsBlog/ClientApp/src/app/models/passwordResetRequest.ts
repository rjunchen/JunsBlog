export class PasswordResetRequest {
    Password: string;
    ResetToken: string;
    Email: string;
    constructor(password: string, resetToken: string, email: string){
        this.Password = password;
        this.ResetToken = resetToken;
        this.Email = email;
    }
  }