import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-eventos-list',
  templateUrl: './eventos-list.component.html',
  styleUrls: ['./eventos-list.component.scss']
})
export class EventosListComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventoId!: number;
  public evento: string = "Eventos";
  private _filtroLista: string = '';
  public exibirImg: boolean = false;
  public eventosFiltrados: Evento[] = [];
  public widthImg: number = 100;
  public marginImg: number = 2;
  public modalRef?: BsModalRef;

  public get filtroLista(): string
  {
      return this._filtroLista;
  }

  public set filtroLista(value: string)
  {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(value) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[]
  {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema!.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
          private eventoService: EventoService,
          private modalService: BsModalService,
          private toastr: ToastrService,
          private spinner: NgxSpinnerService,
          private router: Router) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public getEventos(): void
  {
    this.eventoService.getAll().subscribe({
      next: (eventos: Evento[]) =>{
        this.eventos = eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.toastr.error("Não foi possível carregar os eventos!", "Error")
        console.error(error);
      },
      complete: () => {
        this.toastr.success("Eventos carregados!", "Sucesso");
      }
    }).add(() => this.spinner.hide());
  }

  public detalheEvento(id: number): void
  {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public exibir() : void
  {
    this.exibirImg = !this.exibirImg;
  }

  public openModal(event: any, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void {
    this.modalRef?.hide();

    this.spinner.show();

    this.eventoService.delete(this.eventoId).subscribe(
      (result: any) => {
        if(result.message == 'Deletado')
        {
          this.toastr.success('O evento foi deletado com sucesso!', 'Sucesso!');
          this.spinner.hide();
          this.getEventos();
        }
      },
      (error: any) => {
        this.spinner.hide();
        this.toastr.error("Erro ao deletar o evento", "ERROR");
        console.error(error);
      },
      () => this.spinner.hide()
    );
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public imageURL(imageName: string): string
  {
    return imageName !== '' ? environment.apiURL+'resources/images/'+imageName : 'assets/semImage.png';
  }

}
