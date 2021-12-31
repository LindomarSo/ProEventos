import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UserUpdate } from '@app/models/identity/userUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  public form!: FormGroup;
  public usuario = {} as UserUpdate;
  public perfil: string = "Perfil";
  public file: File[] = [];
  public emptyImage = './assets/semImage.png';
  public urlImage = environment.apiURL + 'Resources/Perfil/';

  public get ehPalestrante(): boolean
  {
    return this.usuario.funcao === 'Palestrante';
  }

  constructor(private accountService: AccountService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService) { }

  public ngOnInit() {

  }

  public setFormValue(user: UserUpdate): void
  {
    this.usuario = user;
    this.emptyImage = this.usuario.imageURL != null ? this.urlImage + this.usuario.imageURL : this.emptyImage;
  }

  public onFileChage(event: any): void
  {
    const reader = new FileReader();

    reader.onload = (evento: any) => this.emptyImage = evento.target.result;

    this.file = event.target.files;

    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  public uploadImage(): void
  {
    this.spinner.show();
    this.accountService.uploadImage(this.file).subscribe(
      (usuario: UserUpdate) => {
        this.usuario.imageURL = usuario.imageURL;
        this.toastr.success("Imagem carregada com sucesso");
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Erro ao realizar o upload');
      }
    ).add(() => this.spinner.hide());
  }
}
