import { Component, OnInit } from '@angular/core';
import { Pagination, PaginationResult } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  public palestrantes: Palestrante[] = [];
  public termBuscaChanged: Subject<string> = new Subject<string>();
  public pagination = { } as Pagination;

  constructor(private toastr: ToastrService,
              private spinner: NgxSpinnerService,
              private palestranteService: PalestranteService) { }

  ngOnInit() {
    this.pagination = { currentPage: 1, itemsPerPage: 10 } as Pagination;
    this.carregarPalestrantes();
  }

  public filtrarEventos(event: any): void
  {
    if(this.termBuscaChanged.observers.length === 0)
    {
      this.spinner.show();
      this.termBuscaChanged.pipe((debounceTime(1000))).subscribe(
        (filtrarPor: any) => {
          this.palestranteService.getAll(this.pagination.currentPage, this.pagination.itemsPerPage, filtrarPor).subscribe(
            (response: PaginationResult<Palestrante[]>) => {
              this.palestrantes = response.result;
              this.pagination = response.pagination;
            },
            (error: any) => {
              console.error(error);
              this.toastr.error('Houve um erro durante o filtro');
            }
          ).add(() => this.spinner.hide());
        }
      )
    }
    this.termBuscaChanged.next(event.value)
  }

  public carregarPalestrantes(): void
  {
    this.spinner.show();
    this.palestranteService.getAll(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (response: PaginationResult<Palestrante[]>) => {
        this.toastr.success('Palestrantes carregados');
        this.palestrantes = response.result;
        this.pagination = response.pagination;
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Houve um error durante o carregamento');
      }
    ).add(() => this.spinner.hide());
  }

  public emptyImage(imageName: string): string
  {
    if(imageName != null)
    {
      return environment.apiURL+'resources/perfil/'+imageName;
    }

    return './assets/semImage.png';
  }
}
