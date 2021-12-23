import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from 'src/environments/environment';
import { Pagination, PaginationResult } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime, throttleTime } from 'rxjs/operators';

@Component({
  selector: 'app-eventos-list',
  templateUrl: './eventos-list.component.html',
  styleUrls: ['./eventos-list.component.scss']
})
export class EventosListComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventoId!: number;
  public evento: string = "Eventos";
  // private _filtroLista: string = '';  usuado apenas para o filtro do lado do cliente
  public exibirImg: boolean = false;
  // public eventosFiltrados: Evento[] = []; usado tambem para filtro do lado cliente
  public widthImg: number = 100;
  public marginImg: number = 2;
  public modalRef?: BsModalRef;
  public pagination = { } as Pagination;
  public termoBuscaChanged: Subject<string> = new Subject<string>();

  // public get filtroLista(): string Usado apenas para o filtro do lado do cliente
  // {
  //     return this._filtroLista;
  // }

  // public set filtroLista(value: string) // Ainda sobre o filtro do lado do cliente
  // {
  //   this._filtroLista = value;
  //   this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(value) : this.eventos;
  // }

  public filtrarEventos(event: any): void
  {
    if(this.termoBuscaChanged.observers.length === 0)
    {
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe( // DEBOUNCETIME É MUITO ÚTIL QUANDO SE QUISER FAZER MUITAS COISAS AUTOMÁTICAS INCLUSIVE SALVAR
        filtrarPor => {
          this.spinner.show();
          this.eventoService.getAll(this.pagination.currentPage, this.pagination.itemsPerPage, filtrarPor).subscribe(
            (response: PaginationResult<Evento[]>) => {
              this.eventos = response.result
              this.pagination = response.pagination;
            },
            (error: any) =>{
              this.toastr.error("Erro ao carregar os eventos");
              console.error(error);
            }
          ).add(() => this.spinner.hide())
        }
      )
    }
    this.termoBuscaChanged.next(event.value); // Tem que ficar depois do if
  }

  constructor(
          private eventoService: EventoService,
          private modalService: BsModalService,
          private toastr: ToastrService,
          private spinner: NgxSpinnerService,
          private router: Router) { }

  public ngOnInit(): void {
    this.pagination = { currentPage: 1, itemsPerPage: 2, totalItems: 1 } as Pagination;
    this.getEventos();
  }

  public getEventos(): void
  {
    this.spinner.show();
    this.eventoService.getAll(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe({
      next: (response: PaginationResult<Evento[]>) =>{
        this.eventos =response.result;
        // this.eventosFiltrados = this.eventos; //  FILTRO DO LADO DO CLIENTE
        this.pagination =response.pagination;
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

  public pageChanged(event: any) : void
  {
    this.pagination.currentPage = event.page;
    this.getEventos();
  }

}
