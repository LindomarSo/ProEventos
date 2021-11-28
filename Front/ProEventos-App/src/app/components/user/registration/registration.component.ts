import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  public user = {} as User;
  public form!: FormGroup;

  public get f(): any
  {
    return this.form.controls;
  }

  constructor(private formBuilder: FormBuilder,
              private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }

  public ngOnInit(): void {
    this.validation();
  }

  public validation(): void
  {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword')
    };

    this.form = this.formBuilder.group({
      primeiroNome: ['', [Validators.required, Validators.minLength(3)]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmarPassword: ['', [Validators.required]]
    }, formOptions);
  }

  public registrar(): void
  {
    this.spinner.show();
    this.user = {...this.form.value };
    this.accountService.register(this.user).subscribe(
      () => { this.router.navigateByUrl('/dashboard'); },
      (error: any) => {
        console.error(error);
        this.toastr.error(error.error);
      }
    ).add(() => this.spinner.hide());
  }
}
