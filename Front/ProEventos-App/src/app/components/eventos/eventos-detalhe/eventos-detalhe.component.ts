import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-eventos-detalhe',
  templateUrl: './eventos-detalhe.component.html',
  styleUrls: ['./eventos-detalhe.component.scss']
})
export class EventosDetalheComponent implements OnInit {

  public form!: FormGroup;
  public evento = {} as Evento;
  public estadoSalvar = 'post';
  public eventoId!: number;
  public modalRef?: BsModalRef;
  public loteAtual = { id: 0, nomwe: '', indice: 0 };
  public imagemURL = '../../../../assets/upload.png';
  public file!: File[];

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private route: ActivatedRoute,
    private eventoService: EventoService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private loteService: LoteService,
    private modalService: BsModalService)
  {
    this.localeService.use("pt-br");
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  public get f(): any
  {
    return this.form.controls;
  }

  public get modeEditar(): boolean
  {
    if(this.estadoSalvar === 'put')
    {
      return true;
    }
    return false;
  }

  public get bsConfig():any
  {
    return {
       adaptivePosition: true,
       dateInputFormat: 'DD/MM/YYYY hh:mm a',
       containerClass: 'theme-default',
       showWeekNumbers: false
    }
  }

  public get bsConfigLote() : any
  {
    return {
       adaptivePosition: true,
       dateInputFormat: 'DD/MM/YYYY',
       containerClass: 'theme-default',
       showWeekNumbers: false
    }
  }

  public get lotes() : FormArray
  {
    return this.form.get('lotes') as FormArray;
  }

  public carregarEvento(): void
  {
    this.eventoId = +this.route.snapshot.paramMap.get('id')!;

    if(this.eventoId !== null && this.eventoId !== 0)
    {
      this.estadoSalvar = 'put';

      this.eventoService.getById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {... evento};
          this.form.patchValue(this.evento); // Carrega todos os campos do formulário
          this.toastr.success("Evento carregado com sucesso!", "Sucesso");
          if(this.evento.imagemURL !== '')
          {
            this.imagemURL = environment.apiURL+'resources/images/'+this.evento.imagemURL;
          }
          // this.carregarLotes();
          evento.lotes.forEach((lote) => {
            this.lotes.push(this.criarLote(lote));
          });
        },
        (error) => {
          this.toastr.error("Erro ao carregar o evento.", "Error");
          console.error(error);
          this.spinner.hide();
        },
        () => this.spinner.hide()
      );
    }
  }

  public carregarLotes() : void
  {
    this.loteService.getLoteByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote));
        });
      },
      (error: any) => {
        this.toastr.error("Erro ao carregar lotes", "Erro");
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }

  public validation(): void
  {
    this.form = this.fb.group({
      local:['', [Validators.required, Validators.minLength(3)]],
      dataEvento:['',  [Validators.required]],
      tema:['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      qtdPessoas:['', [Validators.required, Validators.max(120000)]],
      imagemURL:[''],
      telefone:['', Validators.required],
      email:['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  public adicionarLote(): void
  {
    // Com esta chamada ele considera o ID como 0 e recebe todos os outros parâmetros
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  private criarLote(lote:Lote): FormGroup
  {
    return this.fb.group({
                          id: [lote.id, Validators.required],
                          nomwe: [lote.nomwe, Validators.required],
                          preco: [lote.preco, Validators.required],
                          dataInicio: [lote.dataInicio, Validators.required],
                          dataFim: [lote.dataFim, Validators.required],
                          quantidade: [lote.quantidade, Validators.required]
                        });
  }

  public resetForm(): void
  {
    this.form.reset();
  }

  public resetFormLotes() : void
  {
    this.form.get('lotes')?.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any
  {
    return {'is-invalid': campoForm.errors && campoForm.touched}
  }

  public salvarEvento()
  {
    this.spinner.show();

    if(this.form.valid)
    {

      this.evento = (this.estadoSalvar === 'post' )
                  ? this.evento = {...this.form.value}
                  : {id: this.evento.id, ...this.form.value};

        (this.eventoService as any)[this.estadoSalvar](this.evento).subscribe(
          (evento: Evento): void => {
            this.toastr.success("Evento salvo com sucesso.", "Sucesso!");
            this.router.navigate(['/eventos/detalhe/'+evento.id]);
          },
          (error: any) => {
            console.error(error);
            this.toastr.error("erro ao salvar o evento", "Error");
          }
        ).add(() => this.spinner.hide());
    }
  }

  public salvarLote(): void
  {
    if(this.form.controls.lotes.valid)
    {
      this.spinner.show();
      this.loteService.put(this.form.value.lotes, this.eventoId).subscribe(
        (lote) => {
          this.toastr.success("Lotes salvos com Sucesso!", "Sucesso.");
          // this.lotes.reset();
        },
        (error) => {
          this.toastr.error("Erro ao salvar os lotes", "Error");
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removeLote(indice: number, template: TemplateRef<any>): void
  {
    this.loteAtual.id = this.lotes.get(indice+'.id')?.value;
    this.loteAtual.nomwe = this.lotes.get(indice+'.nomwe')?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void
  {
    this.modalRef?.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso!', 'Sucesso.')
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error) => {
        console.error(error);
        this.toastr.error("Erro ao deletar o lote", "Error");
      }
    ).add(() => this.spinner.hide());
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public retornaTituloLote(campo: string): string
  {
    return campo === null || campo === '' ? 'Nome do lote' : campo;
  }

  public onFileChange(event: any) : void
  {
    const reader = new FileReader();

    reader.onload =  (evento: any) => this.imagemURL = evento.target.result;

    this.file = event.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  public uploadImage(): void
  {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toastr.success("Imagem enviada com sucesso.", "Sucesso!");
      },
      (error) => {
        console.error(error);
        this.toastr.error("Erro ao realizar o upload", "Error");
      }
    ).add(() => this.spinner.hide());
  }
}
