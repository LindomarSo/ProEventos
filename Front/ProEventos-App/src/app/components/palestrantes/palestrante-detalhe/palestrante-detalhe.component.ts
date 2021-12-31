import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {

  public form!: FormGroup;
  public situacaoDoForm = '';
  public corDaDescricao = '';
  public palestrante = { } as Palestrante;

  constructor(private formBuilder: FormBuilder,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private palestranteService: PalestranteService) { }

  ngOnInit() {
    this.validator();
    this.verificaForm();
    this.carregarPalestrante();
  }

  public validator(): void
  {
    this.form = this.formBuilder.group({
      miniCurriculo: ['']
    });
  }

  private verificaForm(): void
  {
    this.form.valueChanges.pipe(map(() => {
      this.situacaoDoForm = 'Minicurriculo está sendo atualizado...';
      this.corDaDescricao = 'text-warning';
    }), debounceTime(1000), tap(/*() => this.spinner.show()*/)).subscribe(
      () => {
        this.palestranteService.put({...this.form.value}).subscribe(
          () => {
            this.situacaoDoForm = 'Minicurriculo foi atualizado';
            this.corDaDescricao = 'text-success'

            setTimeout(() => {
              this.situacaoDoForm = 'Minicurriculo carregado';
              this.corDaDescricao = 'text-muted'
            }, 500)
          },
          (error: any) => {
            console.error(error);
            this.toastr.error('Não foi possível atualizar');
          }
        ).add();
      }
    );
  }

  private carregarPalestrante(): void
  {
    this.spinner.show();
    this.palestranteService.getByUserId().subscribe(
      (response: Palestrante) => {
        this.form.patchValue(response);
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Não foi possível carregar o palestrante');
      }
    ).add(() => this.spinner.hide());
  }
}
