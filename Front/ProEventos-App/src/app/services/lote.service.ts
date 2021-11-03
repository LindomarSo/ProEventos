import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoteService {

  public urlBase = "https://localhost:5001/api/lote";

  constructor(
    private http: HttpClient
  ) { }

  public getLoteByEventoId(id: number): Observable<Lote[]>
  {
    return this.http.get<Lote[]>(`${this.urlBase}/${id}`)
            .pipe(take(1));
  }

  public put(lote: Lote[], eventoId: number) : Observable<Lote[]>
  {
    return this.http.put<Lote[]>(`${this.urlBase}/${eventoId}`, lote)
              .pipe(take(1));
  }

  public deleteLote(eventoId: number, loteId: number) : Observable<any>
  {
    return this.http.delete(`${this.urlBase}/${eventoId}/${loteId}`)
                    .pipe(take(1));
  }
}
