import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/userUpdate';
import { PalestranteUpdate } from '@app/models/PalestranteUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  public form!: FormGroup;
  public userUpdate = {} as UserUpdate;
  public perfil: string = "Perfil";

  @Output() changeFormValue = new EventEmitter();

  public get f(): any
  {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService,
              private router: Router,
              private palestranteService: PalestranteService) { }

  public ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  private verificaForm(): void
  {
    this.form.valueChanges.subscribe(
      () => this.changeFormValue.emit({...this.form.value})
    )
  }

  public onSubmit(): void
  {
    this.atualizarUsuario();
  }

  public atualizarUsuario():void
  {
    this.spinner.show();
    this.userUpdate = {... this.form.value};

    if(this.f.funcao.value == 'Palestrante')
      this.addPalestrante();

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => {
        this.toastr.success('Usuário atualizado com sucesso.');
      },
      (error: any) => {
        this.toastr.error('Não foi possível atualizar o usuário');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  public addPalestrante(): void
  {
    this.palestranteService.post().subscribe(
      () => {
        this.toastr.success('Função palestrante ativada');
      },
      (error: any) => {
        console.error(error)
      }
    )
  }

  public validation(): void
  {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword')
    };

    this.form = this.fb.group({
      userName: [''],
      titulo: ['NaoInformado', [Validators.required]],
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      funcao: ['NaoInformado', [Validators.required]],
      descricao: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmarPassword: ['', [Validators.required]],
      imageURL: [''],
    }, formOptions);
  }

  public cleanScream(event: any): void
  {
    event.preventDefault();
    this.form.reset();
  }

  public carregarUsuario(): void
  {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (user: UserUpdate) => {
        this.userUpdate = user;
        this.form.patchValue(this.userUpdate);
        this.toastr.success('Usuário carregado com sucesso.');
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Erro ao carregar o usuário');
        this.router.navigate(['/dashboard']);
      }
    ).add(() => this.spinner.hide());
  }
}
