import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private accountSercice: AccountService) { }

  public ngOnInit(): void
  {
     this.setCurrentUser();
  }

  public setCurrentUser(): void{
    let user!: User;

    if(localStorage.getItem('user') !== null)
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    else
      user = null!;

    if(user)
      this.accountSercice.setCurrentUser(user)
  }
}
