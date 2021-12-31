import { Component, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UserLogin } from '@app/models/identity/UserLogin';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public model = {} as UserLogin;
  public load = false;

  constructor(private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  public login(): void
  {
    this.load = true;
    this.accountService.login(this.model).subscribe(
      () => { this.router.navigateByUrl('/dashboard'); },
      (error: any) => {
        if(error.status == 401)
          this.toastr.error('Usuário ou senha inválido!', 'Erro!');
        else
          console.error(error);
      }
    ).add(() => this.load = false)
  }
}
