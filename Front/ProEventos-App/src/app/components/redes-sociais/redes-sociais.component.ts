import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RedeSocial } from '@app/models/RedeSocial';
import { RedeSocialService } from '@app/services/redeSocial.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redes-sociais',
  templateUrl: './redes-sociais.component.html',
  styleUrls: ['./redes-sociais.component.scss']
})
export class RedesSociaisComponent implements OnInit {

  public formRS!: FormGroup;
  public bsModel!: BsModalRef;
  public redeSocialAtual = { id: 0, indice: 0, nome: '' };
  @Input() eventoId = 0;

  public get redesSociais(): any
  {
    return this.formRS?.get('redesSociais') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private modalService: BsModalService,
              private redeSocialService: RedeSocialService) { }

  ngOnInit(): void {
    this.validation();
    this.carregarRedeSocial();
  }

  public carregarRedeSocial(): void
  {
    let origem = 'palestrante';

    if(this.eventoId != 0) origem = 'evento';
    this.spinner.show();
    this.redeSocialService.getRedesSociais(origem, this.eventoId).subscribe(
      (redeSocial: RedeSocial[]) => {
        this.toastr.success('Rede Social carregada');
        redeSocial.forEach((rdSocial) => {
          this.redesSociais.push(this.criarRedeSocial(rdSocial));
        });
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Não foi possível carregar as redes sociais');
      }
    ).add(() => this.spinner.hide());
  }

  public validation(): void
  {
    this.formRS = this.formBuilder.group({
      redesSociais: this.formBuilder.array([])
    });
  }

  public addRedeSocial(): void
  {
    this.redesSociais?.push(this.criarRedeSocial({ id: 0 } as RedeSocial))
  }

  public criarRedeSocial(redeSocial: RedeSocial): FormGroup
  {
    return this.formBuilder.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required]
    });
  }

  public salvarRedeSocial(): void
  {
    let origem = 'palestrante';

    if(this.eventoId !== 0) origem = 'evento';

    this.spinner.show();
    this.redeSocialService.salverRedeSocial(origem, this.eventoId, this.formRS.controls.redesSociais.value).subscribe(
      (redeSocial: RedeSocial[]) => {
        this.toastr.success('Rede Social salva com sucesso');
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Não foi posível salvar a rede social');
      }
    ).add(() => this.spinner.hide());
  }

  public removerRedeSocial(): void
  {
    let origem = 'palestrante';

    if(this.eventoId !== 0) origem = 'evento';

    this.spinner.show();
    this.redeSocialService.delete(origem, this.eventoId, this.redeSocialAtual.id).subscribe(
      () => {
        this.toastr.success("Sucesso");
        this.redesSociais.removeAt(this.redeSocialAtual.indice);
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Não foi possível deletar a rede social');
      }
    ).add(() => this.spinner.hide());
  }

  public bsModalRef(template: TemplateRef<any>, indice: number): void
  {
    this.redeSocialAtual.nome = this.redesSociais.get(indice+'.nome')?.value;
    this.redeSocialAtual.id = this.redesSociais.get(indice+'.id')?.value;
    this.redeSocialAtual.indice = indice;

    this.bsModel = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void
  {
    this.bsModel?.hide();
    this.removerRedeSocial();
  }

  public decline(): void
  {
    this.bsModel?.hide();
  }

  public retornaTitulo(nome: string): string
  {
    return nome == null || nome == '' ? 'Rede Social' : nome;
  }
}
